interface News {
  id: number;
  description: string;
  dateCreated: Date;
  title: string;
  markDown: string;
  categories: KeyValuePair[];
  tags: KeyValuePair[];
  userCard: UserProfileCard;
  viewCount: number;
  rating: number;
  comments: CommentCard[];
}
