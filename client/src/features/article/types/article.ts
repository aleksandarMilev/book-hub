export interface CreateArticle {
  title: string;
  introduction: string;
  content: string;
  image?: File | null;
}

export interface ArticleDetails {
  id: string;
  createdOn: string;
  views: number;
  title: string;
  introduction: string;
  content: string;
  imagePath: string;
}
