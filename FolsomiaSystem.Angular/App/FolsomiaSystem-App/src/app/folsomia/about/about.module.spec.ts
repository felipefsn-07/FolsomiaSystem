import { AboutModule } from './about.module';

describe('AboutModule', () => {
  let AboutModule: AboutModule;

  beforeEach(() => {
    AboutModule = new AboutModule();
  });

  it('should create an instance', () => {
    expect(AboutModule).toBeTruthy();
  });
});
