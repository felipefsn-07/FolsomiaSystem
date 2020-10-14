import { ManualModule } from './manual.module';

describe('ManualModule', () => {
  let ManualModule: ManualModule;

  beforeEach(() => {
    ManualModule = new ManualModule();
  });

  it('should create an instance', () => {
    expect(ManualModule).toBeTruthy();
  });
});
