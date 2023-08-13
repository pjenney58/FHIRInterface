import { NextApiRequest, NextApiResponse } from "next";
import NextAuth from "next-auth";
import CredentialsProvider from "next-auth/providers/credentials";


const config = {
  providers: [
    CredentialsProvider({
      name: "Credentials",
      credentials: {
        username: { label: "Username", type: "text", placeholder: "jsmith" },
        password: { label: "Password", type: "password" },
      },
      async authorize(credentials, req) {
        let user = null;
        // TODO - Add logic here to look up the user from the credentials supplied
        if(credentials?.username === "admin" && credentials?.password === "admin") {
          user = { id: "1", name: "Admin", email: "john.barhorst@palisaid.com" };
        }
        return user;
      }
    })
    ]       
  }

export default (req: NextApiRequest, res: NextApiResponse) => NextAuth(req, res, config);
