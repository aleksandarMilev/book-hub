import { Route, Routes } from "react-router-dom";

import Header from './components/header/Header'
import Home from './components/home/Home'
import Footer from './components/footer/Footer'
import Login from './components/identity/Login'
import Register from './components/identity/Register'
import Books from './components/book/Books'
import CreateBook from './components/book/CreateBook'

export default function App(){
    return(
        <>
            <Header />
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route path="/books" element={<Books />} />
                <Route path="/books/create" element={<CreateBook />} />
            </Routes>
            <Footer />
        </>
    );
}