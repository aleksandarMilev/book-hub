interface ArticleBase {
  id: number;
  title: string;
  introduction: string;
  content: string;
  imageUrl?: string | null;
  createdOn?: string;
  views?: number;
}

export type Article = ArticleBase;

export type ArticleInput = Omit<ArticleBase, 'id' | 'createdOn' | 'views'>;

export type ArticleSummary = Pick<
  ArticleBase,
  'id' | 'title' | 'introduction' | 'imageUrl' | 'createdOn'
>;
