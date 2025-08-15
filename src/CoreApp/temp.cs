public async Task Consume(UploadBookmarksMessageRequest message, CancellationToken cancellationToken)
{
    try
    {
        // =================================================================
        // SETUP & PRE-PROCESSING
        // =================================================================
        List<BookmarkDto> uploadedBookmarks = ParseAllBookmarks(message.HtmlContent);
        uploadedBookmarks = RemoveDuplicateBookmarksByUrl(uploadedBookmarks);

        for (int i = 0; i < uploadedBookmarks.Count; i++)
        {
            uploadedBookmarks[i].Id = i + 1;
        }

        var iconsDictionary = uploadedBookmarks.ToDictionary(b => b.Id, b => b.Icon);
        
        // We only need a simplified version for the AI to process.
        var simplifiedBookmarks = uploadedBookmarks
            .Select(b => new { b.Id, b.Title, b.Url })
            .ToList();

        var allCategorizedBookmarks = new List<CategorizationResult>();
        const int batchSize = 150; // A good starting point, adjustable.

        // =================================================================
        // PHASE 1: CATEGORIZE IN BATCHES
        // =================================================================
        // Use .Chunk() to easily create batches (.NET 6+).
        foreach (var batch in simplifiedBookmarks.Chunk(batchSize))
        {
            cancellationToken.ThrowIfCancellationRequested();

            // The JSON payload for this specific batch.
            var jsonDataForBatch = JsonSerializer.Serialize(batch);

            var prompt = @$"For each bookmark in the JSON list below, suggest a single, concise folder name based on its title and URL. 
Group related topics or series under the exact same folder name (e.g., use 'One Piece' for all chapters of that manga). 
If a bookmark is unique, suggest 'Miscellaneous'.
Return ONLY a JSON array of objects, where each object has the original 'Id' (int) and a new 'FolderName' (string) property.

Data:
{jsonDataForBatch}";

            // Call the new, simpler AI method.
            try
            {
                var batchResult = await _aiService.CategorizeBookmarksAsync(prompt, cancellationToken);
                if (batchResult != null)
                {
                    allCategorizedBookmarks.AddRange(batchResult);
                }
            }
            catch (Exception ex)
            {
                // Log error for this specific batch and decide whether to continue or fail.
                Console.WriteLine($"Error processing a batch: {ex.Message}");
                // For now, we'll continue with the successful batches.
            }
        }

        // =================================================================
        // PHASE 2: CONSOLIDATE RESULTS
        // =================================================================
        // Create a lookup dictionary for easy access to the folder names.
        var folderNameLookup = allCategorizedBookmarks.ToDictionary(r => r.Id, r => r.FolderName);

        var finalResponse = new BookmarkGroupingResponse
        {
            WithFolder = new List<Folder>(),
            WithoutFolder = new List<BookmarkDto>()
        };

        // Group the *original* bookmarks using the AI-generated folder names.
        var groupedBookmarks = uploadedBookmarks
            .Where(b => folderNameLookup.ContainsKey(b.Id))
            .GroupBy(b => folderNameLookup[b.Id]);

        foreach (var group in groupedBookmarks)
        {
            // Handle sub-folders by splitting on '/'
            string[] folderPath = group.Key.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            
            if (group.Key.Equals("Miscellaneous", StringComparison.OrdinalIgnoreCase) || folderPath.Length == 0)
            {
                finalResponse.WithoutFolder.AddRange(group);
            }
            else
            {
                // This logic adds bookmarks to a nested folder structure.
                AddBookmarksToNestedFolder(finalResponse.WithFolder, folderPath, group.ToList());
            }
        }
        
        // =================================================================
        // FINALIZATION
        // =================================================================
        // Re-attach icons to the final, structured data.
        foreach (var bookmark in finalResponse.WithoutFolder)
        {
            if (iconsDictionary.TryGetValue(bookmark.Id, out var icon))
            {
                bookmark.Icon = icon;
            }
        }
        foreach (var folder in finalResponse.WithFolder)
        {
            SetIconsInFolder(folder, iconsDictionary);
        }

        // TODO: Convert 'finalResponse' to an importable HTML file.
        // TODO: Save the resulting file to blob storage.
        // TODO: Update database with the job status and file URL.
        // TODO: Notify the user via SignalR.
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Critical error processing upload {message.UploadId}: {ex.Message}");
        // Handle final failure, update DB status to "Failed".
    }
}

// Helper method to handle nested folder creation.
private void AddBookmarksToNestedFolder(List<Folder> folders, string[] path, List<BookmarkDto> bookmarks)
{
    var currentFolders = folders;
    Folder targetFolder = null;

    foreach (var folderName in path)
    {
        targetFolder = currentFolders.FirstOrDefault(f => f.Name.Equals(folderName, StringComparison.OrdinalIgnoreCase));
        if (targetFolder == null)
        {
            targetFolder = new Folder { Name = folderName, Bookmarks = new List<BookmarkDto>(), SubFolders = new List<Folder>() };
            currentFolders.Add(targetFolder);
        }
        currentFolders = targetFolder.SubFolders;
    }
    
    if (targetFolder != null)
    {
        targetFolder.Bookmarks.AddRange(bookmarks);
    }
}