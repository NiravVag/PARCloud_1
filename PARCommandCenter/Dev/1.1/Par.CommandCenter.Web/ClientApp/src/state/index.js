import React, { createContext, useState, useEffect, useContext } from 'react';


const stateContextDefaultValue = {
    app: {
        isLoading: true,
    },
    user: {
        userId: 0,
        userName: "",
        email: "",
        tenantIds: [],
        isAuthorized: false,
        isUserFirstLogin: false,
    },
    onAppStateRefresh: () => {}
}

const stateContext = createContext(stateContextDefaultValue);

export default function AppStateProvider(props) {
    // The useState() hook defines a state variable.
    const [appStateData, setAppStateData] = useState(stateContextDefaultValue);

    const [doRefresh, setDoRefresh] = useState(0);

    const handleRefresh = () => {
        setDoRefresh(doRefresh + 1);
    }

    // The useEffect() hook registers a function to run after render.
    useEffect(() => {
        fetch('api/Application/State')        // Ask the server for user data.
            .then(response => response.json()) // Get the response as JSON
            .then(data => {                    // When data arrives...
                return setAppStateData(({
                    app: {
                        isLoading: false
                    },
                    user: {
                        userId: data.userId,
                        userName: data.userName,
                        email: data.email,
                        tenantIds: data.tenantIds,
                        isAuthorized: data.isAuthorized,
                        isUserFirstLogin: data.isUserFirstLogin,
                    },
                    onAppStateRefresh: handleRefresh
                }));
            })
            .catch(function () {
                setAppStateData(({
                    app: {
                        isLoading: false
                    },
                    user: {
                        userId: 0,
                        userName: "",
                        email: "",
                        tenantIds: [],
                        isAuthorized: false,
                    },
                    onAppStateRefresh: handleRefresh
                }));

                console.log("error");
            });
    }, [doRefresh]);

    return (
        <stateContext.Provider value={appStateData}>
            {props.children}
        </stateContext.Provider>
    );
}

export function useAppState() {
    const context = useContext(stateContext);
    if (!context) {
        throw new Error('useAppState must be used within the AppStateProvider');
    }
    return context;
}