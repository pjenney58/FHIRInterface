import AccessDenied from "components/AccessDenied";
import { useSession } from "next-auth/react";

export default function Billing() {
  const { data: session } = useSession();

  if (!session) {
    return (
      <AccessDenied />
    )
  }

  return (
    <div>
      <h1>Billing</h1>
    </div>
  )
}
