// Google JSON Style Guide https://google.github.io/styleguide/jsoncstyleguide.xml?showone=data#data

// Succes Response
// {
//   "data": {
//     "id": 1001,
//     "name": "Wing"
//   }
// }

// Error Response
// {
//   "apiVersion": "2.0",
//   "error": {
//     "code": 404,
//     "message": "File Not Found",
//     "errors": [{
//       "domain": "Calendar",
//       "reason": "ResourceNotFoundException",
//       "message": "File Not Found
//     }]
//   }
// }


export interface ApiError {
  domain?: string;
  reason?: string;
  message?: string;
}

export interface ApiTopLevelError {
  code: number;
  message?: string;
  errors: ApiError[];
}

export interface ApiErrorResponse {
  apiVersion: string;
  error: ApiTopLevelError;
}
