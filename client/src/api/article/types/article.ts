interface ArticleCore {
  title: string;
  introduction: string;
  imageUrl?: string | null;
}

interface ArticleContent {
  content: string;
}

interface ArticleMetadata {
  id: number;
  createdOn?: string | null;
  views?: number | null;
}

export interface ArticleBase extends ArticleCore, ArticleContent, ArticleMetadata {}

export type Article = ArticleBase;

export interface ArticleSummary extends ArticleCore, Pick<ArticleMetadata, 'id' | 'createdOn'> {}

export interface ArticleInput extends ArticleCore, ArticleContent {}
