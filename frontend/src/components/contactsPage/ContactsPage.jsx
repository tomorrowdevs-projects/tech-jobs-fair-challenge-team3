import React, { useState, useEffect } from "react";
import DropdownFilters from "../UI/DropdownFilters";
import ContactsList from "./ContactsList";
import Searchbar from "./Searchbar";

const ContactPage = ({query}) => {
    const [result, setResult] = useState()
    const [contacts, setContacts] = useState([]);
    const [filter, setFilter] = useState("");
    const [currentPage, setCurrentPage] = useState(0);
    const [pageIndex, setIndex] = useState(currentPage + 1)
    const pageSize = 10; // Numero di contatti per pagina
    const [totalContacts, setTotalContacts] = useState(0); // Numero totale di contatti

    // Effetto per caricare i contatti quando la pagina cambia
    useEffect(() => {
        // Simulazione della richiesta API per ottenere i contatti
        const fetchContacts = async () => {
            let data = {}; // Definizione di data all'inizio della funzione
            try {
                const response = await fetch(
                    `https://tjf-challenge.azurewebsites.net/web/people/list`,
                    {
                        headers: {
                            Authorization:
                                "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI5YmQzOGYxOC1lNjBjLTQ4NDItODFlMS0zMGZiZTY0NjA1YWYiLCJpYXQiOjE3MTIzOTAzMjQsImlkIjoiYTFjMTZmZjctZGY5YS00MWZiLWIyMjgtMjQwYTRjOTc0NjE2IiwiZnVsbG5hbWUiOiJBZG1pbiBUSkYtQ2hhbGxlbmdlIiwidXNlcm5hbWUiOiJhZG1pbiIsInJvbGUtaWQiOiIyMjJjZmY5ZC0xZjJkLTRmNWYtYmEyYi05YzUxOTgzYmQ0MGQiLCJyb2xlIjoiQWRtaW4iLCJuYmYiOjE3MTIzOTAzMjQsImV4cCI6MTcxMjM5MTIyNCwiaXNzIjoiVG9tb3Jyb3dEZXZzIiwiYXVkIjoiVG9tb3Jyb3dEZXZzIn0.9Khnd5I6nb7zm3RLvf79rLb4ttj5UP53xjDKlLoSwH4",
                            Accept: "application/json",
                            "Content-Type": "application/json",
                        },
                        method: "POST",
                        body: JSON.stringify({
                            page: currentPage,
                        })
                    }
                );
        
                if (!response.ok) {
                    throw new Error(`HTTP error! Status: ${response.status}`);
                }
        
                data = await response.json(); // Assegnamento del valore a data
                
                setTotalContacts(data.recordsTotal);
        
                // Assicurati che totalContacts sia un numero positivo
                const total = parseInt(data.recordsTotal);
                setTotalContacts(total);
                
                const contatti = parseInt(data.data)
                setContacts(contatti);
                
            } catch (error) {
                console.error("Error fetching data:", error);
                console.error("Response data:", data);
            }
        }
       

        fetchContacts();
    }, [currentPage]);


    // Funzione per gestire il cambio di pagina
    const handlePageChange = (pageNumberIndex, pageNumber) => {
        setCurrentPage(pageNumber)
        setIndex(pageNumberIndex); // Imposta il numero di pagina corrente
    }

    



    // Funzione per generare i pulsanti delle pagine
    const generatePageButtons = () => {
        const totalPages = Math.ceil(totalContacts / pageSize);
        const pages = [];
        for (let i = 1; i <= totalPages; i++) {
            pages.push(
                <li key={i}>
                    <button
                        className={`flex items-center justify-center text-sm py-2 px-3 leading-tight text-gray-500 bg-white border border-gray-300 focus:outline-none ${
                          pageIndex === i
                                ? "hover:bg-gray-100 hover:text-gray-700 dark:bg-gray-800 dark:border-gray-700 dark:text-gray-400 dark:hover:bg-gray-700 dark:hover:text-white"
                                : "hover:bg-gray-100 hover:text-gray-700 dark:bg-gray-800 dark:border-gray-700 dark:text-gray-400 dark:hover:bg-gray-700 dark:hover:text-white"
                        }`}
                        onClick={() => handlePageChange(i, i - 1)}
                    >
                        {i}
                    </button>
                </li>
            );
        }
        return pages;
    };
    
        const handleSearch = (value) => {
            console.log("Searched for:", value);
        };

    return (
        <section className="bg-vivid dark:bg-vivid p-3 sm:p-5 min-h-screen">
            <div className="mx-auto max-w-screen-xl px-4 lg:px-12">
                <div
                    className="bg-neutral dark:bg-vivid relative shadow-md sm:rounded-lg overflow-hidden"
                    style={{ minHeight: "650px" }}
                >
                    <div className="container bg-neutral dark:bg-vivid flex flex-wrap justify-between p-4">
                        <div className="flex flex-col sm:flex-row w-full sm:w-auto">
                        <Searchbar setResult={setResult} filter={filter} onSearch={handleSearch} />
                            <DropdownFilters
                                filter={filter}
                                setFilter={setFilter}
                            />
                        </div>
                        <div className="flex justify-center sm:justify-end mt-4 sm:mt-0">
                            <button
                                id="add-product"
                                aria-expanded="true"
                                aria-haspopup="true"
                                type="button"
                                className="flex items-center justify-center text-neutral bg-subdue hover:vivid focus:ring-4 focus:ring-vivid font-medium rounded-lg text-sm px-4 py-2 dark:subdue dark:hover:vivid focus:outline-none dark:focus:ring-vivid"
                            >
                                <svg
                                    className="h-3.5 w-3.5 mr-2"
                                    fill="currentColor"
                                    viewBox="0 0 20 20"
                                    xmlns="http://www.w3.org/2000/svg"
                                    aria-hidden="true"
                                >
                                    <path
                                        clipRule="evenodd"
                                        fillRule="evenodd"
                                        d="M10 3a1 1 0 011 1v5h5a1 1 0 110 2h-5v5a1 1 0 11-2 0v-5H4a1 1 0 110-2h5V4a1 1 0 011-1z"
                                    />
                                </svg>
                                Add Contact
                            </button>
                        </div>
                    </div>
                    <div
                        className="overflow-x-auto"
                        style={{ minHeight: "500px" }}
                    >
                        <table className="w-full text-sm text-left text-vivid dark:text-neutral">
                            <thead className="text-xs text-neutral uppercase bg-subdue dark:bg-subdue dark:text-neutral">
                                <tr>
                                    <th scope="col" className="px-4 py-3">
                                        First Name
                                    </th>
                                    <th scope="col" className="px-4 py-3">
                                        Last Name
                                    </th>
                                    <th scope="col" className="px-4 py-3">
                                        Phone
                                    </th>
                                    <th scope="col" className="px-4 py-3">
                                        Address
                                    </th>
                                    <th
                                        scope="col hidden lg:flex"
                                        className="px-4 py-3"
                                    >
                                        Social Account
                                    </th>
                                    <th
                                        scope="col hidden lg:flex"
                                        className="px-4 py-3"
                                    >
                                        <span className="sr-only">Actions</span>
                                    </th>
                                </tr>
                            </thead>
                            <ContactsList
                                contacts={contacts}
                                currentPage={currentPage}
                                pageSize={pageSize}
                                setContacts={setContacts}
                            />
                        </table>
                    </div>
                    <nav
                        className="flex flex-col md:flex-row justify-between items-start md:items-center space-y-3 md:space-y-0 p-4"
                        aria-label="Table navigation"
                    >
                        <span className="text-sm font-normal text-vivid dark:text-neutral">
                            Showing

                            <span className="font-semibold text-gray-900 dark:text-white">
                                 {Math.min(
                                    pageIndex * pageSize, totalContacts
                                )}
                            </span>

                            of

                            <span className="font-semibold text-gray-900 dark:text-white">
                                 {totalContacts} 
                            </span>
                        </span>
                        <ul className="inline-flex items-stretch -space-x-px">
                            <li>
                                <button
                                    className={`flex items-center justify-center h-full py-1.5 px-3 ml-0 text-gray-500 bg-white rounded-l-lg border border-gray-300 focus:outline-none ${
                                        pageIndex === 1
                                            ? "cursor-not-allowed"
                                            : "hover:bg-gray-100 hover:text-gray-700 dark:bg-gray-800 dark:border-gray-700 dark:text-gray-400 dark:hover:bg-gray-700 dark:hover:text-white"
                                    }`}
                                    onClick={() =>
                                        handlePageChange(pageIndex - 1, currentPage - 1)
                                    }
                                    disabled={pageIndex === 1}
                                >
                                    <span className="sr-only">Previous</span>
                                    <svg
                                        className="w-5 h-5"
                                        aria-hidden="true"
                                        fill="currentColor"
                                        viewBox="0 0 20 20"
                                        xmlns="http://www.w3.org/2000/svg"
                                    >
                                        <path
                                            fillRule="evenodd"
                                            d="M12.707 5.293a1 1 0 010 1.414L9.414 10l3.293 3.293a1 1 0 01-1.414 1.414l-4-4a1 1 0 010-1.414l4-4a1 1 0 011.414 0z"
                                            clipRule="evenodd"
                                        />
                                    </svg>
                                </button>
                            </li>
                            {generatePageButtons()}
                            <li>
                                <button
                                    className={`flex items-center justify-center h-full py-1.5 px-3 leading-tight text-gray-500 bg-white rounded-r-lg border border-gray-300 focus:outline-none ${
                                        pageIndex === Math.ceil(totalContacts / pageSize) 
                                            ? "cursor-not-allowed"
                                            : "hover:bg-gray-100 hover:text-gray-700 dark:bg-gray-800 dark:border-gray-700 dark:text-gray-400 dark:hover:bg-gray-700 dark:hover:text-white"
                                    }`}
                                    onClick={() =>
                                        handlePageChange(pageIndex + 1 , currentPage + 1)
                                    }
                                    disabled={
                                        pageIndex ===
                                        Math.ceil(totalContacts / pageSize)
                                    }
                                >
                                    <span className="sr-only">Next</span>
                                    <svg
                                        className="w-5 h-5"
                                        aria-hidden="true"
                                        fill="currentColor"
                                        viewBox="0 0 20 20"
                                        xmlns="http://www.w3.org/2000/svg"
                                    >
                                        <path
                                            fillRule="evenodd"
                                            d="M7.293 14.707a1 1 0 010-1.414L10.586 10 7.293 6.707a1 1 0 011.414-1.414l4 4a1 1 0 010 1.414l-4 4a1 1 0 01-1.414 0z"
                                            clipRule="evenodd"
                                        />
                                    </svg>
                                </button>
                            </li>
                        </ul>
                    </nav>
                </div>
            </div>
        </section>
    )
}

export default ContactPage;