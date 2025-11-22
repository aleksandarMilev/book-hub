export interface CreateArticle {
  title: string;
  introduction: string;
  content: string;
  image?: File | null | undefined;
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
