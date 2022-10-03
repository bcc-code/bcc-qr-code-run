import {reactive} from 'vue'
import baseUrl from "./baseApi";

export interface Team {
    teamName: string,
    churchName: string,
    members: string,
    score: number,
    posts: [],
    timeSpent: string,
    secretsFound: []
}

export const store = reactive({
    team: null as Team|null,
    isLoggedIn: false,
    errorMessage: "",
    loggingIn: false
})

export async function login(teamName: string, churchName: string, members: number) {
    if (store.loggingIn) return;
    store.loggingIn = true;
    try {
        const result = await fetch(`${baseUrl}/team/register`, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            credentials: "include",
            referrerPolicy: "strict-origin-when-cross-origin",
            mode: "cors",
            body: JSON.stringify({
                "teamName": teamName,
                "churchName": churchName,
                "members": members
            })
        });
        if(result.ok) {
            store.team = (await result.json()) as Team;
            store.isLoggedIn = store.team !== null;
            store.errorMessage = "";
            return store.team;
        }
        else
        {
            store.errorMessage = await result.json();
            return null
        }
    } catch (e) {
        
    }
    finally
    {
        store.loggingIn = false;
    }
}

export async function getLoggedInTeam() {
    const result = await fetch(`${baseUrl}/team/`, {
        credentials: "include",
    });
    if (result.ok) {
        store.team = await result.json();
        store.isLoggedIn = store.team !== null;
        store.errorMessage = "";
        return store.team;
    } else {
        store.team = null;
        store.isLoggedIn = false;
    }
}

export async function logout() {
    try {
        const result = await fetch(`${baseUrl}/team/logout`, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            credentials: "include",
        });
        store.team = null;
        store.isLoggedIn = false;
    } catch (e) { }
}
