import { HttpClient, provideHttpClient, withInterceptors } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { API_URL, apiUrlInterceptor } from './api-url-interceptor';

describe('ApiUrlInterceptor', () => {
  let httpMock: HttpTestingController;
  let http: HttpClient;
  const apiUrl = 'https://foo.bar/api';

  const setApiUrl = (url: string | null) => {
    TestBed.overrideProvider(API_URL, { useValue: url });
    httpMock = TestBed.inject(HttpTestingController);
    http = TestBed.inject(HttpClient);
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        { provide: API_URL, useValue: null },
        provideHttpClient(withInterceptors([apiUrlInterceptor])),
        provideHttpClientTesting(),
      ],
    });
  });

  afterEach(() => httpMock.verify());

  it('should not prepend API url when API url is empty', () => {
    setApiUrl(null);

    http.get('/user').subscribe(data => expect(data).toEqual({ success: true }));

    httpMock.expectOne('/user').flush({ success: true });
  });

  it('should prepend API url when request url does not has http scheme', () => {
    setApiUrl(apiUrl);

    http.get('./user').subscribe(data => expect(data).toEqual({ success: true }));
    httpMock.expectOne(apiUrl + '/user').flush({ success: true });

    http.get('').subscribe(data => expect(data).toEqual({ success: true }));
    httpMock.expectOne(apiUrl).flush({ success: true });
  });
});
