interface News {
  id: number;
  title: string;
  content: string;
  visitorCount: number;
  createdAt: Date;
  modifiedAt: Date;
  userName: string;
  userId: string;
  markDown: string;
  categories: KeyValuePair[];
  tags: KeyValuePair[];
  userCard: UserProfileCard;
  rating: number;
  comments: CommentCard[];
}
