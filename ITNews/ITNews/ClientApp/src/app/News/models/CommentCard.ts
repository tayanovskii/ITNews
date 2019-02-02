interface CommentCard {
  id: number;
  newsId: number;
  content: string;
  userCard: UserProfileCard;
  countLikes: number;
  createdAt: Date;
  modifiedAt: Date;
  modifiedBy: string;
}
