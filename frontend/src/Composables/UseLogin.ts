import {reactive} from 'vue'

const baseUrl = "https://localhost:7065"

interface Team {
    teamName: string,
    churchName: string,
    members: string,
}

export const store = reactive({
    team: null as Team|null,
    isLoggedIn: false,
})

export async function login(teamName: string, churchName: string, members: number) {
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
        store.team = (await result.json()) as Team;
        store.isLoggedIn = store.team !== null;
        return store.team;
    } catch (e) {
        
    }
}

export async function getLoggedInTeam() {
    const result = await fetch(`${baseUrl}/team/`, {
        credentials: "include",
    });
    store.team = await result.json();
    store.isLoggedIn = store.team !== null;
    return store.team;
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

//
// const login = async () => {
//     try {
//         const result = await fetch('https://localhost:7065/team/register', {
//             method: 'POST',
//             headers: {
//                 'Accept': 'application/json',
//                 'Content-Type': 'application/json'
//             },
//             credentials: "include",
//             referrerPolicy: "strict-origin-when-cross-origin",
//             mode: "cors",
//             body: JSON.stringify({
//                 "groupCode": "tbg",
//                 "teamName": "globetrotters",
//                 "teamMemberCount": 0})
//         });
//     }
//     catch (e){ }
// }
//
// const getLoggedInTeam = async () => {
//     const result = await fetch('https://localhost:7065/team/', {
//         credentials: "include",
//     });
//     return await result.json()
// }
//
//
// const logout = async () => {
//     try {
//         const result = await fetch('https://localhost:7065/team/logout', {
//             method: 'POST',
//             headers: {
//                 'Accept': 'application/json',
//                 'Content-Type': 'application/json'
//             },
//             credentials: "include",
//         });
//         reply.value = await result.json();
//     }
//     catch (e){ }
// }