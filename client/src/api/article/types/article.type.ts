export type ISODateString = string;

interface ArticleBase {
  id: number;
  title: string;
  introduction: string;
  content: string;
  imageUrl?: string | null;
  createdOn?: ISODateString;
  views?: number;
}

export type Article = ArticleBase;

export type ArticleInput = Omit<ArticleBase, 'id' | 'createdOn' | 'views'>;

export type ArticleSummary = Pick<
  ArticleBase,
  'id' | 'title' | 'introduction' | 'imageUrl' | 'createdOn'
>;
