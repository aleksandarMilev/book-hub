import ArticleForm from '../ArticleForm'

import * as useArticle from '../../../hooks/useArticle'

import DefaultSpinner from '../../common/default-spinner/DefaultSpinner'

export default function EditArticle(){
    const { article } = useArticle.useDetails()

    return profile 
        ? <ArticleForm article={article} isEditMode={true} /> 
        : <DefaultSpinner/ >
}