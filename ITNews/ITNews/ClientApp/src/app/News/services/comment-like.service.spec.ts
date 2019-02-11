import { TestBed } from '@angular/core/testing';

import { CommentLikeService } from './comment-likes.service';

describe('LikesService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CommentLikeService = TestBed.get(CommentLikeService);
    expect(service).toBeTruthy();
  });
});
