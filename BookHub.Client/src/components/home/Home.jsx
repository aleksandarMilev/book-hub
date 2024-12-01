import Hero from "./hero/Hero"
import TopAuthors from "./top-authors/TopAuthors"
import TopBooks from "./top-books/TopBooks"
import TopUsers from "./top-users/TopUsers"

export default function Home(){
    return(
        <>
            <Hero />
            <TopAuthors />
            <TopBooks />
            <TopUsers />
        </>
    )
}