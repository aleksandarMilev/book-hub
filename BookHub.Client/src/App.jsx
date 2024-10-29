import { Route, Routes } from "react-router-dom";

import Header from './components/header/Header'
import Home from './components/home/Home'
import Footer from './components/footer/Footer'
import Login from './components/identity/Login'
import Register from './components/identity/Register'
import Books from './components/book/Books'
import CreateBook from './components/book/CreateBook'
import { routes } from "./common/constants";

export default function App(){
    return(
        <>
            <Header />
            <Routes>
                <Route path={routes.home} element={<Home />} />
                <Route path={routes.login} element={<Login />} />
                <Route path={routes.register} element={<Register />} />
                <Route path={routes.books} element={<Books />} />
                <Route path={routes.createBook} element={<CreateBook />} />
            </Routes>
            <Footer />
        </>
    );
}