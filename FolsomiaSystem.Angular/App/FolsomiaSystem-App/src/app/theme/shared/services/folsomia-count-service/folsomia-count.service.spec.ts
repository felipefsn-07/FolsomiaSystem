import { TestBed } from '@angular/core/testing';

import { FolsomiaCountService } from './folsomia-count.service';

describe('FolsomiaCountService', () => {
  let service: FolsomiaCountService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FolsomiaCountService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
