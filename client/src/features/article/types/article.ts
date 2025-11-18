export interface CreateArticle {
  title: string;
  introduction: string;
  content: string;
  imageUrl: string | null;
}

export interface ArticleDetails extends CreateArticle {
  id: string;
  createdOn: string;
  views: number;
  title: string;
  introduction: string;
  content: string;
  imageUrl: string;
}
