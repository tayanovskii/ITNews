import { TestBed, async, inject } from '@angular/core/testing';

import { WriterRoleGuard } from './writer-role.guard';

describe('WriterRoleGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [WriterRoleGuard]
    });
  });

  it('should ...', inject([WriterRoleGuard], (guard: WriterRoleGuard) => {
    expect(guard).toBeTruthy();
  }));
});
