
Of the text provided, make the best summary possible trying to keep minimal text but keeping a good brief of the notes, as i will use them to study later. Give the summary in .md file: 

# Cert

https://www.datacamp.com/tutorial/azure-openai

https://www.reddit.com/r/AzureCertification/comments/1lailo1/passed_ai102_leaving_few_tips_here/

https://www.reddit.com/r/AzureCertification/comments/1nf3z15/passed_ai102_some_thoughts_and_one_big_tip/

https://certs.msfthub.wiki/azure/ai-102/

https://learn.microsoft.com/en-us/credentials/certifications/azure-ai-engineer/?WT.mc_id=studentamb_165290&practice-assessment-type=certification




# Ollama

- API Reference
https://github.com/ollama/ollama/blob/main/docs/api.md


## Pull

ollama pull codellama:code
ollama pull mistral
ollama pull llama3.2
ollama pull qwen2.5

## Run

Gemma 3	4B	3.3GB	
ollama run gemma3


## Docs

https://www.postman.com/ai-on-postman/postman-ai-agent-builder/collection/4u9pkl2/ollama-rest-api


https://www.postman.com/ai-engineer/generative-ai-apis/collection/k68agqe/ollama-api-localhost

## Open Web UI



## Chatterbox

- APIs / Server apps using chatterbox

1- 
https://github.com/devnen/Chatterbox-TTS-Server

2- 
https://github.com/travisvn/chatterbox-tts-api

3- 
https://www.reddit.com/r/SteamDeck/comments/1iknilv/llms_run_surprisingly_well_on_steam_decks_due_to/ 


### Commands

1- Libraries

sudo apt install libsndfile1
sudo apt install ffmpeg

## For docker run

# Use Docker-optimized environment variables
cp .env.example.docker .env  # Docker-specific paths, ready to use
# Or: cp .env.example .env   # Local development paths, needs customization

# Choose your deployment method:

# API Only (default)
docker compose -f docker/docker-compose.yml up -d             # Standard (pip-based)
docker compose -f docker/docker-compose.uv.yml up -d          # uv-optimized (faster builds)
docker compose -f docker/docker-compose.gpu.yml up -d         # Standard + GPU
docker compose -f docker/docker-compose.uv.gpu.yml up -d      # uv + GPU (recommended for GPU users)
docker compose -f docker/docker-compose.cpu.yml up -d         # CPU-only
docker compose -f docker/docker-compose.blackwell.yml up -d   # Blackwell (50XX) NVIDIA GPUs

# API + Frontend (add --profile frontend to any of the above)
docker compose -f docker/docker-compose.yml --profile frontend up -d             # Standard + Frontend
docker compose -f docker/docker-compose.gpu.yml --profile frontend up -d         # GPU + Frontend
docker compose -f docker/docker-compose.uv.gpu.yml --profile frontend up -d      # uv + GPU + Frontend
docker compose -f docker/docker-compose.blackwell.yml --profile frontend up -d   # (Blackwell) uv + GPU + Frontend

# Watch the logs as it initializes (the first use of TTS takes the longest)
docker logs chatterbox-tts-api -f

# Test the API
curl -X POST http://localhost:4123/v1/audio/speech \
  -H "Content-Type: application/json" \
  -d '{"input": "Hello from Chatterbox TTS!"}' \
  --output test.wav


## For local run

### With UV

- Install uv if you haven't already
curl -LsSf https://astral.sh/uv/install.sh | sh

- Install dependencies with uv (automatically creates venv)
uv sync

- Copy and customize environment variables
cp .env.example .env

- Start the API with FastAPI
uv run uvicorn app.main:app --host 0.0.0.0 --port 4123

-  Or use the main script
uv run server.py

### With pip

- Check Python commands to Puthon local setup

- Run in each session
source .venv/bin/activate

- Exit
deactivate 

2- Only CPU Install

```bash
# Make sure your (venv) is active
pip install --upgrade pip
pip install -r requirements.txt
```
3- Run server

```bash
python server.py
```
