import { Category } from './../../Shared/models/category';
import { Tag } from 'src/app/Shared/models/tag';
export interface News {
  id: number;
  title: string;
  content: string;
  createdAt: Date;
  modifiedAt: Date;
  markDown: string;
  categories: Category[];
  tags: Tag[];
  newsStatistic: NewsStat;
  userMiniCardDto: UserProfileCard;
  comments: CommentCard[];
}
