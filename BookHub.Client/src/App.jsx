import { Route, Routes } from "react-router-dom";

import { apiRoutes } from "./common/constants";

import Header from './components/header/Header'
import Home from './components/home/Home'
import Footer from './components/footer/Footer'
import Login from './components/identity/Login'
import Register from './components/identity/Register'
import CreateBook from './components/book/CreateBook'
import BookList from "./components/book/BookList";
import BookDetails from "./components/book/BookDetails";

export default function App(){
    return(
        <>
             <Header />
            <Routes>
                <Route path={apiRoutes.home} element={<Home />} />
                <Route path={apiRoutes.login} element={<Login />} />
                <Route path={apiRoutes.register} element={<Register />} />
                <Route path={apiRoutes.books} element={<BookList />} />
                <Route path={apiRoutes.createBook} element={<CreateBook />} />
                <Route path={apiRoutes.bookDetails} element={<BookDetails />} />
            </Routes>
            <Footer /> 
        </>
    );
}