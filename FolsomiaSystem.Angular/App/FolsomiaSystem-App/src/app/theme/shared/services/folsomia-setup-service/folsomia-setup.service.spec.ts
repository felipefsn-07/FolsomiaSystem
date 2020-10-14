import { TestBed } from '@angular/core/testing';

import { FolsomiaSetupService } from './folsomia-setup.service';

describe('FolsomiaSetupService', () => {
  let service: FolsomiaSetupService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FolsomiaSetupService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
