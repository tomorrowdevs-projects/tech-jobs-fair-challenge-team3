import { Dialog, Transition } from "@headlessui/react"
import { Fragment, useState } from "react"
import { ToastContainer, toast } from "react-toastify"

const ContactAddModal = ({ modalToggle, setModalToggle }) => {
    const [name, setName] = useState("")
    const [surname, setSurname] = useState("")
    const [role, setRole] = useState("")
    const [address, setAddress] = useState("")
    const [phone, setPhone] = useState("")
    const [email, setEmail] = useState("")
    const [photoUrl, setPhotoUrl] = useState("")

    const handlePhotoChange = (e) => {
        const file = e.target.files[0]
        const reader = new FileReader()
        reader.onloadend = () => {
            setPhotoUrl(reader.result)
        }
        reader.readAsDataURL(file)
    }

    const newContact = {
        firstname: name,
        lastname: surname,
        role: role,
        address: address,
        phoneNumber: phone,
        email: email,
    }
    const handleAddContact = async (e) => {
        e.preventDefault()
        const fetchPromise = new Promise(async (resolve, reject) => {
            try {
                const response = await fetch(
                    "https://tjf-challenge.azurewebsites.net/web/people/insert",
                    {
                        headers: { "Content-Type": "application/json" },
                        method: "POST",
                        body: JSON.stringify(newContact),
                    }
                )
                if (!response) {
                    reject(new Error(`HTTP error! Status: ${response.status}`))
                } else {
                    const data = await response.json()
                    resolve(data)
                    // Close the modal

                    setModalToggle(false)
                }
            } catch (error) {
                console.error("Error fetching data:", error)
            }
        })
        toast.promise(fetchPromise, {
            pending: "Loading...",
            success: "Add new contact!",
            error: "Something went wrong",
        })
    }

    return (
        <>
            <Transition appear show={modalToggle} as={Fragment}>
                <Dialog
                    as="div"
                    className="relative z-10 text-subdue"
                    onClose={() => setModalToggle(false)}
                >
                    <Transition.Child
                        as={Fragment}
                        enter="ease-out duration-300"
                        enterFrom="opacity-0"
                        enterTo="opacity-100"
                        leave="ease-in duration-200"
                        leaveFrom="opacity-100"
                        leaveTo="opacity-0"
                    >
                        <div className="fixed inset-0 bg-black/25" />
                    </Transition.Child>

                    <div className="fixed inset-0 overflow-y-auto">
                        <div className="flex min-h-full items-center justify-center p-4 text-center">
                            <Transition.Child
                                as={Fragment}
                                enter="ease-out duration-300"
                                enterFrom="opacity-0 scale-95"
                                enterTo="opacity-100 scale-100"
                                leave="ease-in duration-200"
                                leaveFrom="opacity-100 scale-100"
                                leaveTo="opacity-0 scale-95"
                            >
                                <div className="card">
                                    <div className="flex justify-between">
                                        <button
                                            className="p-2"
                                            onClick={() =>
                                                setModalToggle(false)
                                            }
                                        >
                                            <svg
                                                xmlns="http://www.w3.org/2000/svg"
                                                width="16"
                                                height="16"
                                                fill="currentColor"
                                                class="bi bi-x"
                                                viewBox="0 0 16 16"
                                            >
                                                <path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708" />
                                            </svg>
                                        </button>
                                    </div>

                                    <form
                                        onSubmit={handleAddContact}
                                        className="card-edit-form"
                                    >
                                        <div
                                            className="photo-input"
                                            onClick={() =>
                                                document
                                                    .getElementById(
                                                        "photo-upload"
                                                    )
                                                    .click()
                                            }
                                        >
                                            <img
                                                className="card-edit-avatar"
                                                src={
                                                    photoUrl ||
                                                    "https://placehold.jp/150x150.png"
                                                }
                                                alt="avatar"
                                            />
                                            <input
                                                id="photo-upload"
                                                type="file"
                                                accept="image/*"
                                                style={{ display: "none" }}
                                                onChange={handlePhotoChange}
                                            />
                                        </div>
                                        <input
                                            type="text"
                                            placeholder="Name"
                                            value={name}
                                            onChange={(e) =>
                                                setName(e.target.value)
                                            }
                                        />
                                        <input
                                            type="text"
                                            placeholder="Surname"
                                            value={surname}
                                            onChange={(e) =>
                                                setSurname(e.target.value)
                                            }
                                        />
                                        <input
                                            type="text"
                                            placeholder="Role"
                                            value={role}
                                            onChange={(e) =>
                                                setRole(e.target.value)
                                            }
                                        />
                                        <input
                                            type="text"
                                            placeholder="Address"
                                            value={address}
                                            onChange={(e) =>
                                                setAddress(e.target.value)
                                            }
                                        />
                                        <input
                                            type="text"
                                            placeholder="Phone"
                                            value={phone}
                                            onChange={(e) =>
                                                setPhone(e.target.value)
                                            }
                                        />
                                        <input
                                            type="email"
                                            placeholder="Email"
                                            value={email}
                                            onChange={(e) =>
                                                setEmail(e.target.value)
                                            }
                                        />
                                        <button type="submit">Save</button>
                                    </form>
                                </div>
                            </Transition.Child>
                        </div>
                    </div>
                </Dialog>
            </Transition>
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
}

export default ContactAddModal
