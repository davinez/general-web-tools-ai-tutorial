export * from './noop-interceptor';
export * from './api-url-interceptor';
export * from './settings-interceptor';
export * from './token-interceptor';
export * from './api-interceptor';
export * from './error-interceptor';
export * from './logging-interceptor';

import { apiInterceptor } from './api-interceptor';
import { apiUrlInterceptor } from './api-url-interceptor';
import { errorInterceptor } from './error-interceptor';
import { loggingInterceptor } from './logging-interceptor';
import { noopInterceptor } from './noop-interceptor';
import { settingsInterceptor } from './settings-interceptor';
import { tokenInterceptor } from './token-interceptor';

// Http interceptor providers in outside-in order
export const interceptors = [
  noopInterceptor,
  apiUrlInterceptor,
  settingsInterceptor,
  tokenInterceptor,
  apiInterceptor,
  errorInterceptor,
  loggingInterceptor,
];
