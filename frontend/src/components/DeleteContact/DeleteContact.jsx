import React, { forwardRef, useEffect, useState } from "react"
import { ToastContainer, toast } from "react-toastify"
import "react-toastify/dist/ReactToastify.css"
import { usePage } from "../../context/PageContext"
import useContacts from "../../hooks/fetchData"

const DeleteContact = forwardRef((props, ref) => {
    const {
        selectedContactId,
        active,
        activeClass,
        noActiveClass,
        customClass,
    } = props
    const { currentPage } = usePage()

    const { fetchContacts } = useContacts(currentPage)
    const [deletedContacts, setDeletedContacts] = useState(false)

    useEffect(() => {
        if (deletedContacts) {
            fetchContacts()
        }
    }, [deletedContacts, fetchContacts])

    //Confirm before deleting
    const handleConfirm = async () => {
        if (!selectedContactId) {
            return
        }
        if (window.confirm("Are you sure you want to delete the contact?")) {
            await handleDelete()
        }
    }
    //Request to delete contact
    const handleDelete = async () => {
        let formData = new FormData()
        formData.append("id", selectedContactId)

        const fetchPromise = new Promise(async (resolve, reject) => {
            try {
                const response = await fetch(
                    "https://tjf-challenge.azurewebsites.net/web/people/delete",
                    {
                        method: "DELETE",
                        body: formData,
                    }
                )

                if (response.ok) {
                    const data = await response.json()
                    resolve(data)
                    setDeletedContacts(true)
                } else {
                    reject(new Error(`HTTP error! Status: ${response.status}`))
                }
            } catch (error) {
                reject(error)
            }
        })

        toast.promise(fetchPromise, {
            pending: "Loading...",
            success: "Deleted!",
            error: "Something went wrong",
        })
    }
    console.log(deletedContacts)
    return (
        <>
            <button
                onClick={handleConfirm}
                className={`${
                    active ? activeClass : noActiveClass
                } hover:bg-subdue hover:text-neutral  ${customClass}`}
            >
                Delete
            </button>
            <ToastContainer
                position="bottom-right"
                autoClose={5000}
                hideProgressBar={false}
                newestOnTop={false}
                closeOnClick
                rtl={false}
                pauseOnFocusLoss
                draggable
                pauseOnHover
                theme="light"
            />
        </>
    )
})
export default DeleteContact
