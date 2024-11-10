import { MDBContainer, MDBListGroup } from 'mdb-react-ui-kit'

import * as useBook from '../../hooks/useBook'

import BookListItem from './BooksListItem'
import DefaultSpinner from '../common/DefaultSpinner'

export default function BookList() {
    //const { books, isFetching } = useBook.useGetAll()

    const books = [
        {
            id: 1,
            title: 'Pet Sematary',
            author: 'Stephen King',
            shortDescription: 'A terrifying novel from the King of horror. longerlongerlongerlongerlongerlongerlongerlongerlonger' + 
            'longetlongetlongetlongetlongetlongetlongetlongetlongetlongetlongetlongetlongetlongetlongetlongetlonget',
            longDescription: 'A tale of grief, loss, and supernatural terror. The Creed family faces unimaginable consequences when they bury their beloved pet in an ancient burial ground.',
            rating: 4.67,
            image: 'https://m.media-amazon.com/images/I/91ndIrptO4L._AC_UF1000,1000_QL80_.jpg',
            genres: ['horror, thriller']
        },
        {
            id: 2,
            title: 'The Silmarillion',
            author: 'J.R.R. Tolkien',
            shortDescription: 'Introduction to the world of Lord of the Rings.',
            longDescription: 'This book delves into the ancient and mythic world of Middle-earth, telling the stories of its creation, the battles of its first ages, and the rise and fall of its greatest kingdoms.',
            rating: 4.12,
            image: 'https://g.christianbook.com/dg/product/cbd/f400/338012.jpg',
            genres: ['fantasy']
        },
        {
            id: 3,
            title: 'Harry Potter and the Deathly Hallows',
            author: 'J.K. Rowling',
            shortDescription: 'The boy who lived versus the One whose name should not be pronounced.',
            longDescription: 'The final installment in the Harry Potter series, where Harry, Ron, and Hermione embark on a dangerous mission to find and destroy Voldemort’s Horcruxes, leading to the ultimate battle of good versus evil.',
            rating: 4.81,
            image: 'https://m.media-amazon.com/images/I/81aCMT1zKtL._AC_UF1000,1000_QL80_.jpg',
            genres: ['fantasy']
        }
    ]

    return (
            <div class="container mt-5 mb-5">
                <div class="d-flex justify-content-center row">
                    <div class="col-md-10">
                {5 === 4 
                    ?  <DefaultSpinner/> 
                    :  books.length > 0 
                            ? books.map(b => (<BookListItem key={b.id} {...b}/>))
                            : <p className="text-center mt-4 text-muted">No books found!</p>
                }
                    </div>
                </div>
            </div>
    )
}
