export interface CreateArticle {
  title: string;
  introduction: string;
  content: string;
  imageUrl?: string | null;
}

export interface ArticleDetails extends CreateArticle {
  id: number;
  createdOn: string;
  views: number;
}
