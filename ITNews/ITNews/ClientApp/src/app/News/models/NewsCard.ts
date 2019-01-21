interface NewsCard {
  id: number;
  title: string;
  dateCreated: Date;
  description: string;
  categories: KeyValuePair[];
  viewCount: number;
  tags: KeyValuePair[];
  userCard: UserProfileCard;
  rating: number;
  commentsCount: number;
}
