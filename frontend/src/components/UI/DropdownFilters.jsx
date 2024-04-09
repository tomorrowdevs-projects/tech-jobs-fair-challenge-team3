import { Listbox, Transition } from "@headlessui/react"
import { CheckIcon } from "@heroicons/react/20/solid"
import { Fragment, useState } from "react"

const filters = [
    { name: "Employees" },
    { name: "Clients" },
    { name: "Partner" },
]
export default function DropdownFilters(props) {
    const { setFilter } = props
    const [selected, setSelected] = useState(filters[0])

    return (
        <div className="relative inline-block text-left">
            <Listbox value={selected} onChange={setSelected}>
                <div className="relative mt-1">
                    <Listbox.Button className="w-full md:w-auto flex items-center justify-center py-2 px-4 text-sm font-medium text-neutral focus:outline-none bg-subdue rounded-lg border border-vivid hover:bg-vivid hover:text-white focus:z-10 focus:ring-4 focus:ring-slate-800 dark:focus:ring-subdue dark:bg-subdue dark:text-neutral dark:border-subdue dark:hover:text-white">
                        <svg
                            xmlns="http://www.w3.org/2000/svg"
                            aria-hidden="true"
                            className="h-4 w-4 mr-2 text-outline"
                            viewBox="0 0 20 20"
                            fill="currentColor"
                        >
                            <path
                                fillRule="evenodd"
                                d="M3 3a1 1 0 011-1h12a1 1 0 011 1v3a1 1 0 01-.293.707L12 11.414V15a1 1 0 01-.293.707l-2 2A1 1 0 018 17v-5.586L3.293 6.707A1 1 0 013 6V3z"
                                clipRule="evenodd"
                            />
                        </svg>
                        Filters{" "}
                        <svg
                            className="-mr-1 ml-1.5 w-5 h-5"
                            fill="currentColor"
                            viewBox="0 0 20 20"
                            xmlns="http://www.w3.org/2000/svg"
                            aria-hidden="true"
                        >
                            <path
                                clipRule="evenodd"
                                fillRule="evenodd"
                                d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z"
                            />
                        </svg>
                    </Listbox.Button>
                    <Transition
                        as={Fragment}
                        leave="transition ease-in duration-100"
                        leaveFrom="opacity-100"
                        leaveTo="opacity-0"
                    >
                        <Listbox.Options className="fixed mt-1 max-h-40 w-52 z-index-50 overflow-auto rounded-md bg-subdue text-neutral py-1 text-base shadow-lg ring-1 ring-vivid/5 focus:outline-none sm:text-sm">
                            {filters.map((filter, filterIdx) => (
                                <Listbox.Option
                                    key={filterIdx}
                                    className={({ active }) =>
                                        `relative cursor-default select-none py-2 pl-10 pr-4 ${
                                            active
                                                ? "bg-vivid text-neutral"
                                                : "text-white"
                                        }`
                                    }
                                    value={filter}
                                    onChange={(e) => setFilter(e.target.value)}
                                >
                                    {({ selected }) => (
                                        <>
                                            <span
                                                className={`block truncate ${
                                                    selected
                                                        ? "font-medium"
                                                        : "font-normal"
                                                }`}
                                            >
                                                {filter.name}
                                            </span>
                                            {selected ? (
                                                <span className="absolute inset-y-0 left-0 flex items-center pl-3 text-neutral">
                                                    <CheckIcon
                                                        className="h-5 w-5"
                                                        aria-hidden="true"
                                                    />
                                                </span>
                                            ) : null}
                                        </>
                                    )}
                                </Listbox.Option>
                            ))}
                        </Listbox.Options>
                    </Transition>
                </div>
            </Listbox>
        </div>
    )
}
