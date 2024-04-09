import { useState, useEffect, useCallback } from "react"

const useContacts = (currentPage) => {
    const [localContacts, setLocalContacts] = useState([])
    const [loading, setLoading] = useState(false)
    const [error, setError] = useState(null)
    const [totContacts, setTotContacts] = useState()
    const fetchContacts = useCallback(async () => {
        setLoading(true)
        setError(null)
        try {
            const response = await fetch(
                `https://tjf-challenge.azurewebsites.net/web/people/list`,
                {
                    headers: {
                        Authorization: "Bearer your_token_here",
                        Accept: "application/json",
                        "Content-Type": "application/json",
                    },
                    method: "POST",
                    body: JSON.stringify({
                        page: currentPage,
                    }),
                }
            )

            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`)
            } else {
                const data = await response.json()
                setLocalContacts(data.data)
                setTotContacts(data.recordsTotal)
            }
        } catch (error) {
            console.error("Error fetching data:", error)
            setError(error)
        } finally {
            setLoading(false)
        }
    }, [currentPage])

    useEffect(() => {
        fetchContacts()
    }, [fetchContacts])

    return {
        localContacts,
        setLocalContacts,
        setLoading,
        totContacts,
        loading,
        error,
        fetchContacts,
    }
}

export default useContacts
