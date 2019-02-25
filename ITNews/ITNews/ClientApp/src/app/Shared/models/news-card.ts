import { Category } from './category';
import { Tag } from './tag';

export class NewsCard {
  id: number;
  title: string;
  description: string;
  content: string;
  createdAt: Date;
  modifiedAt: Date;
  categories: Category[];
  tags: Tag[];
  user: UserProfileCard;
  newsStatistic: NewsStat;
}
