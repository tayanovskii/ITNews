interface SaveNews {
  id?: number;
  title: string;
  description: string;
  categories: KeyValuePair [];
  tags: KeyValuePair [];
  content: string;
  userId: string;
}
