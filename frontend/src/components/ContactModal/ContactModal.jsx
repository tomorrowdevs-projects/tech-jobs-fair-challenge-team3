import { Dialog, Transition } from "@headlessui/react"
import { Fragment, useCallback, useEffect, useState } from "react"
import { DotLoader } from "react-spinners"
import DeleteContact from "../DeleteContact/DeleteContact"
import "./ContactsModal.css" // Import the CSS file containing styles

const CardComponent = (props) => {
    const { modalToggle, setModalToggle, selectedContactId } = props
    const [contactDetails, setContactDetails] = useState("")

    const [showEditCard, setShowEditCard] = useState(false)
    const [editMode, setEditMode] = useState(false)
    const [name, setName] = useState()
    const [role, setRole] = useState()
    const [address, setAddress] = useState()
    const [phone, setPhone] = useState()
    const [email, setEmail] = useState()
    const [photoUrl, setPhotoUrl] = useState(
        "https://thispersondoesnotexist.com/"
    )
    const [loading, setLoading] = useState(false)
    useEffect(() => {
        // Simula il caricamento dei dati, Solo in produzione
        setLoading(true)
        setTimeout(() => {
            setLoading(false)
        }, 5000)
    }, [setLoading])
    const EditCard = ({ onEdit, onCancel }) => {
        return (
            <div className="edit-card">
                {onCancel ? (
                    <button onClick={onCancel}>Cancel</button>
                ) : (
                    <button onClick={onEdit}>Edit</button>
                )}
                <DeleteContact selectedContactId={selectedContactId} />
            </div>
        )
    }
    const getContactIcon = (type) => {
        const icons = {
            "Mobile phone": () => (
                <svg
                    xmlns="http://www.w3.org/2000/svg"
                    viewBox="0 0 24 24"
                    fill="none"
                    stroke="currentColor"
                    strokeWidth="2"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                >
                    <path d="M22 16.92v3a2 2 0 01-2.18 2 19.79 19.79 0 01-8.63-3.07 19.5 19.5 0 01-6-6 19.79 19.79 0 01-3.07-8.67A2 2 0 014.11 2h3a2 2 0 012 1.72 12.84 12.84 0 00.7 2.81 2 2 0 01-.45 2.11L8.09 9.91a16 16 0 006 6l1.27-1.27a2 2 0 012.11-.45 12.84 12.84 0 002.81.7A2 2 0 0122 16.92z" />
                </svg>
            ),
            Email: () => (
                <svg
                    xmlns="http://www.w3.org/2000/svg"
                    viewBox="0 0 24 24"
                    fill="none"
                    stroke="currentColor"
                    strokeWidth="2"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                >
                    <path d="M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z" />
                    <path d="M22 6l-10 7L2 6" />
                </svg>
            ),
            Address: () => (
                <svg
                    xmlns="http://www.w3.org/2000/svg"
                    viewBox="0 0 24 24"
                    fill="none"
                    stroke="currentColor"
                    strokeWidth="2"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                >
                    <path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0118 0z" />
                    <circle cx="12" cy="10" r="3" />
                </svg>
            ),
        }

        return icons[type] ? icons[type]() : null
    }

    function closeModal() {
        setModalToggle(false)
        setEditMode(false)
        setShowEditCard(false)
    }

    const handleEditClick = () => {
        setShowEditCard(!showEditCard)
    }

    const handlePhotoChange = (e) => {
        const file = e.target.files[0]
        const reader = new FileReader()
        reader.onloadend = () => {
            setPhotoUrl(reader.result)
        }
        reader.readAsDataURL(file)
    }

    const handleEdit = () => {
        setEditMode(true)
        setShowEditCard(false)
    }

    const handleCancel = () => {
        setEditMode(false)
    }

    const handleSave = () => {
        setEditMode(false)
    }

    const fetchContact = useCallback(async () => {
        if (!selectedContactId) {
            return
        }
        try {
            const response = await fetch(
                `https://tjf-challenge.azurewebsites.net/web/people/get/${selectedContactId}`
            )
            if (!response) {
                throw new Error(`HTTP error! Status: ${response.status}`)
            }
            const data = await response.json()
            setContactDetails(data)
        } catch (error) {
            console.error("Error fetching data:", error)
        } finally {
            setLoading(false)
        }
    }, [selectedContactId])

    useEffect(() => {
        if (modalToggle && selectedContactId) {
            fetchContact()
        }
    }, [modalToggle, selectedContactId, fetchContact])

    return (
        <>
            <Transition appear show={modalToggle} as={Fragment}>
                <Dialog
                    as="div"
                    className="relative z-10 text-subdue"
                    onClose={closeModal}
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
                                            onClick={closeModal}
                                        >
                                            <svg
                                                xmlns="http://www.w3.org/2000/svg"
                                                width="16"
                                                height="16"
                                                fill="currentColor"
                                                className="bi bi-x"
                                                viewBox="0 0 16 16"
                                            >
                                                <path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708" />
                                            </svg>
                                        </button>

                                        <div className="card-edit-button">
                                            <button
                                                className="edit-button"
                                                onClick={handleEditClick}
                                                aria-label="Edit"
                                            >
                                                <svg
                                                    xmlns="http://www.w3.org/2000/svg"
                                                    height="1em"
                                                    fill="currentColor"
                                                    viewBox="0 0 512 512"
                                                >
                                                    <path d="M 396 25 Q 406 16 419 16 L 419 16 L 419 16 Q 431 16 441 25 L 487 71 L 487 71 Q 496 81 496 93 Q 496 106 487 116 L 420 183 L 420 183 L 329 92 L 329 92 L 396 25 L 396 25 Z M 318 103 L 409 194 L 318 103 L 409 194 L 166 437 L 166 437 Q 163 440 161 442 L 161 407 L 161 407 Q 160 400 153 399 L 113 399 L 113 399 L 113 359 L 113 359 Q 112 352 105 351 L 70 351 L 70 351 Q 72 349 75 346 L 318 103 L 318 103 Z M 52 384 Q 55 375 59 367 L 97 367 L 97 367 L 97 407 L 97 407 Q 98 414 105 415 L 145 415 L 145 415 L 145 453 L 145 453 Q 137 457 128 460 L 21 491 L 21 491 L 52 384 L 52 384 Z M 453 14 Q 438 0 419 0 L 419 0 L 419 0 Q 399 0 385 14 L 63 335 L 63 335 Q 45 354 37 379 L 1 501 L 1 501 Q 0 505 3 509 Q 7 512 11 511 L 133 475 L 133 475 Q 158 467 177 449 L 498 127 L 498 127 Q 512 113 512 93 Q 512 74 498 59 L 453 14 L 453 14 Z M 319 172 Q 323 166 319 161 Q 313 156 307 161 L 163 305 L 163 305 Q 159 310 163 316 Q 169 321 175 316 L 319 172 L 319 172 Z" />
                                                </svg>
                                            </button>

                                            {showEditCard && (
                                                <EditCard
                                                    onEdit={handleEdit}
                                                    onCancel={
                                                        editMode
                                                            ? handleCancel
                                                            : null
                                                    }
                                                />
                                            )}
                                        </div>
                                    </div>

                                    {editMode ? (
                                        <div className="card-edit-form">
                                            <div className="photo-input">
                                                <img
                                                    className="card-edit-avatar"
                                                    src={photoUrl}
                                                    alt="avatar"
                                                />
                                                <label htmlFor="photo-upload">
                                                    <span>Change Photo</span>
                                                    <input
                                                        id="photo-upload"
                                                        type="file"
                                                        accept="image/*"
                                                        onChange={
                                                            handlePhotoChange
                                                        }
                                                    />
                                                </label>
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
                                            <button onClick={handleSave}>
                                                Save
                                            </button>
                                        </div>
                                    ) : loading ? (
                                        <div className="flex items-center justify-center min-h-[50vh]">
                                            <DotLoader color="#3d6098" />
                                        </div>
                                    ) : (
                                        <>
                                            <div className="card-header">
                                                <img
                                                    className="card-avatar"
                                                    src="https://thispersondoesnotexist.com/"
                                                    alt="avatar"
                                                />
                                                <h1 className="card-fullname">
                                                    <span className="me-1">
                                                        {
                                                            contactDetails.firstname
                                                        }
                                                    </span>
                                                    <span>
                                                        {" "}
                                                        {
                                                            contactDetails.lastname
                                                        }
                                                    </span>
                                                </h1>
                                                <h2 className="card-roletitle">
                                                    {role}
                                                </h2>
                                            </div>
                                            <div className="card-main">
                                                <div className="card-social">
                                                    <a href="/">
                                                        <svg
                                                            viewBox="0 0 24 24"
                                                            xmlns="http://www.w3.org/2000/svg"
                                                        >
                                                            <path d="M15.997 3.985h2.191V.169C17.81.117 16.51 0 14.996 0c-3.159 0-5.323 1.987-5.323 5.639V9H6.187v4.266h3.486V24h4.274V13.267h3.345l.531-4.266h-3.877V6.062c.001-1.233.333-2.077 2.051-2.077z" />
                                                        </svg>
                                                    </a>
                                                    <a
                                                        href="/"
                                                        aria-label="Social link"
                                                    >
                                                        <svg
                                                            xmlns="http://www.w3.org/2000/svg"
                                                            viewBox="0 0 512 512"
                                                        >
                                                            <path d="M512 97.248c-19.04 8.352-39.328 13.888-60.48 16.576 21.76-12.992 38.368-33.408 46.176-58.016-20.288 12.096-42.688 20.64-66.56 25.408C411.872 60.704 384.416 48 354.464 48c-58.112 0-104.896 47.168-104.896 104.992 0 8.32.704 16.32 2.432 23.936-87.264-4.256-164.48-46.08-216.352-109.792-9.056 15.712-14.368 33.696-14.368 53.056 0 36.352 18.72 68.576 46.624 87.232-16.864-.32-33.408-5.216-47.424-12.928v1.152c0 51.008 36.384 93.376 84.096 103.136-8.544 2.336-17.856 3.456-27.52 3.456-6.72 0-13.504-.384-19.872-1.792 13.6 41.568 52.192 72.128 98.08 73.12-35.712 27.936-81.056 44.768-130.144 44.768-8.608 0-16.864-.384-25.12-1.44C46.496 446.88 101.6 464 161.024 464c193.152 0 298.752-160 298.752-298.688 0-4.64-.16-9.12-.384-13.568 20.832-14.784 38.336-33.248 52.608-54.496z" />
                                                        </svg>
                                                    </a>
                                                    <a
                                                        href="/"
                                                        aria-label="Social link"
                                                    >
                                                        <svg
                                                            viewBox="0 0 512 512"
                                                            xmlns="http://www.w3.org/2000/svg"
                                                        >
                                                            <path d="M301 256c0 24.852-20.148 45-45 45s-45-20.148-45-45 20.148-45 45-45 45 20.148 45 45zm0 0" />
                                                            <path d="M332 120H180c-33.086 0-60 26.914-60 60v152c0 33.086 26.914 60 60 60h152c33.086 0 60-26.914 60-60V180c0-33.086-26.914-60-60-60zm-76 211c-41.355 0-75-33.645-75-75s33.645-75 75-75 75 33.645 75 75-33.645 75-75 75zm86-146c-8.285 0-15-6.715-15-15s6.715-15 15-15 15 6.715 15 15-6.715 15-15 15zm0 0" />
                                                            <path d="M377 0H135C60.562 0 0 60.563 0 135v242c0 74.438 60.563 135 135 135h242c74.438 0 135-60.563 135-135V135C512 60.562 451.437 0 377 0zm45 332c0 49.625-40.375 90-90 90H180c-49.625 0-90-40.375-90-90V180c0-49.625 40.375-90 90-90h152c49.625 0 90 40.375 90 90zm0 0" />
                                                        </svg>
                                                    </a>
                                                    <a
                                                        href="/"
                                                        aria-label="Social link"
                                                    >
                                                        <svg
                                                            viewBox="0 0 24 24"
                                                            xmlns="http://www.w3.org/2000/svg"
                                                        >
                                                            <path d="M23.994 24v-.001H24v-8.802c0-4.306-.927-7.623-5.961-7.623-2.42 0-4.044 1.328-4.707 2.587h-.07V7.976H8.489v16.023h4.97v-7.934c0-2.089.396-4.109 2.983-4.109 2.549 0 2.587 2.384 2.587 4.243V24zM.396 7.977h4.976V24H.396zM2.882 0C1.291 0 0 1.291 0 2.882s1.291 2.909 2.882 2.909 2.882-1.318 2.882-2.909A2.884 2.884 0 002.882 0z" />
                                                        </svg>
                                                    </a>
                                                </div>
                                            </div>
                                            {contactDetails.contactsList?.map(
                                                (contact, index) => (
                                                    <div
                                                        key={index}
                                                        className="card-contacts"
                                                    >
                                                        <div className="card-contact">
                                                            {getContactIcon(
                                                                contact.type
                                                            )}
                                                            {contact.contact}
                                                        </div>
                                                    </div>
                                                )
                                            )}
                                        </>
                                    )}
                                </div>
                            </Transition.Child>
                        </div>
                    </div>
                </Dialog>
            </Transition>
        </>
    )
}
export default CardComponent
