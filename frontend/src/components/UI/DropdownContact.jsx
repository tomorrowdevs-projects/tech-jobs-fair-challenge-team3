import { Menu, Transition } from "@headlessui/react"
import { Fragment, useState } from "react"
import ContactModal from "./../ContactModal/ContactModal"
import DeleteContact from "./../DeleteContact/DeleteContact"
function classNames(...classes) {
    return classes.filter(Boolean).join(" ")
}

export default function Dropdown(props) {
    const { contactId } = props
    let [modalToggle, setModalToggle] = useState(false)
    const [selectedContactId, setSelectedContactId] = useState()
    function handleOnclick() {
        setModalToggle(true)
        setSelectedContactId(contactId)
    }
    return (
        <>
            <Menu as="div" className="relative inline-block text-left">
                <div>
                    <Menu.Button
                        className="inline-flex items-center p-0.5 text-sm font-medium text-center text-gray-500 hover:text-gray-800 rounded-lg focus:outline-none dark:text-gray-400 dark:hover:text-gray-100"
                        onClick={() => {
                            setSelectedContactId(contactId)
                        }}
                    >
                        <svg
                            className="w-5 h-5"
                            aria-hidden="true"
                            fill="currentColor"
                            viewBox="0 0 20 20"
                            xmlns="http://www.w3.org/2000/svg"
                        >
                            <path d="M6 10a2 2 0 11-4 0 2 2 0 014 0zM12 10a2 2 0 11-4 0 2 2 0 014 0zM16 12a2 2 0 100-4 2 2 0 000 4z" />
                        </svg>
                    </Menu.Button>
                </div>

                <Transition
                    as={Fragment}
                    enter="transition ease-out duration-100"
                    enterFrom="transform opacity-0 scale-95"
                    enterTo="transform opacity-100 scale-100"
                    leave="transition ease-in duration-75"
                    leaveFrom="transform opacity-100 scale-100"
                    leaveTo="transform opacity-0 scale-95"
                >
                    <Menu.Items
                        className="absolute z-10 w-44 bg-neutral rounded divide-y divide-vivid shadow dark:bg-neutral dark:divide-subdue"
                        style={{ left: "-150px" }}
                    >
                        <div className="flex flex-col py-1 text-sm text-vivid dark:text-neutral">
                            <Menu.Item>
                                {({ active }) => (
                                    <button
                                        onClick={handleOnclick}
                                        className={classNames(
                                            active
                                                ? "bg-subdue text-white"
                                                : "text-vivid",
                                            "block px-4 py-2 text-sm "
                                        )}
                                    >
                                        Info
                                    </button>
                                )}
                            </Menu.Item>
                            <Menu.Item>
                                {({ active }) => (
                                    <DeleteContact
                                        active={active}
                                        activeClass="bg-subdue text-white"
                                        noActiveClass="text-vivid"
                                        customClass="block px-4 py-2 text-sm"
                                        selectedContactId={selectedContactId}
                                    />
                                )}
                            </Menu.Item>
                        </div>
                    </Menu.Items>
                </Transition>
                <ContactModal
                    modalToggle={modalToggle}
                    setModalToggle={setModalToggle}
                    selectedContactId={selectedContactId}
                />
            </Menu>
        </>
    )
}
