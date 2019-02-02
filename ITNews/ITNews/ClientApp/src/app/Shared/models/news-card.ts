import { Category } from './category';
import { Tag } from './tag';

export class NewsCard {
  id: number;
  title: string;
  description: string;
  createdAt: Date;
  modifiedAt: Date;
  categories: Category[];
  tags: Tag[];
  user: UserProfileCard;
  newsStatistic: NewsStat;
}
