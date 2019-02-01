export class NewsCard {
  id: number;
  title: string;
  description: string;
  visitorCount: number;
  createdAt: Date;
  modifiedAt: Date;
  categories: KeyValuePair[];
  tags: KeyValuePair[];
  userCard: UserProfileCard;
  rating: number;
  commentCount: number;
}
