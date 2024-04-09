import { useState } from "react"
import useContacts from "../../hooks/fetchData"
import { usePage } from "../../context/PageContext"

const Searchbar = () => {
    const [query, setQuery] = useState("")
    const { currentPage } = usePage()

    const { setLocalContacts } = useContacts(currentPage)
    const handleSearch = async (e) => {
        e.preventDefault()

        const searchPayload = {
            firstname: query,
        }

        try {
            const response = await fetch(
                `https://tjf-challenge.azurewebsites.net/web/people/list`,
                {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify(searchPayload),
                }
            )

            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`)
            }

            const data = await response.json()
            setLocalContacts(data.data)
        } catch (error) {
            console.error("Error fetching data:", error)
        }
    }
    return (
        <div className="flex items-center">
            <label htmlFor="simple-search" className="sr-only">
                Search
            </label>
            <form onSubmit={handleSearch}>
                <div className="relative w-full">
                    <div className="absolute inset-y-0 left-0 flex items-center pl-3 pointer-events-none">
                        <svg
                            aria-hidden="true"
                            className="w-5 h-5 text-subdue dark:text-neutral"
                            fill="currentColor"
                            viewBox="0 0 20 20"
                            xmlns="http://www.w3.org/2000/svg"
                        >
                            <path
                                fillRule="evenodd"
                                d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z"
                                clipRule="evenodd"
                            />
                        </svg>
                    </div>
                    <input
                        type="text"
                        id="simple-search"
                        className="bg-neutral border border-subdue text-vivid text-sm outline-none rounded-lg focus:ring-subdue focus:border-vivid block w-full pl-10 p-2 dark:bg-vivid dark:border-subdue dark:placeholder-neutral dark:text-white dark:focus:ring-subdue dark:focus:border-subdue"
                        placeholder="Search"
                        required=""
                        value={query}
                        onChange={(e) => setQuery(e.target.value)}
                    />
                </div>
            </form>
        </div>
    )
}
export default Searchbar
