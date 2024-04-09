import React, { useEffect } from "react"
import useContacts from "../../hooks/fetchData"
import DropdownContact from "./../UI/DropdownContact"
import { usePage } from "../../context/PageContext"

const ContactList = () => {
    const { currentPage } = usePage()

    const { localContacts, loading, error, fetchContacts } =
        useContacts(currentPage)

    useEffect(() => {
        fetchContacts()
    }, [fetchContacts])

    return (
        <tbody>
            {loading && <p>Loading...</p>}
            {error && <p>Error: {error.message}</p>}

            {localContacts.map((contact, i) => (
                <tr
                    key={`item-${i}`} // Key is correctly placed here
                    className="border-b border-subdue dark:border-subdue"
                >
                    <th
                        scope="row"
                        className="px-4 py-3 font-medium text-vivid whitespace-nowrap dark:text-white"
                    >
                        {contact.firstname}
                    </th>

                    <td className="px-4 py-3">{contact.lastname}</td>
                    <td className="hidden md:table-cell px-4 py-3">
                        {contact.phoneNumber}
                    </td>
                    <td className="hidden md:table-cell px-4 py-3">
                        {contact.email}
                    </td>
                    <td className="hidden lg:table-cell px-4 py-3">
                        {contact.socialAccount}
                    </td>
                    <td className="relative px-4 py-3 flex items-center justify-end">
                        <DropdownContact contactId={contact.id} />
                    </td>
                </tr>
            ))}
        </tbody>
    )
}

export default ContactList
