/* eslint-disable @typescript-eslint/no-unsafe-argument */
/* eslint-disable @typescript-eslint/no-unsafe-member-access */
import { useEffect, useState } from "react";
import Sidebar from "../components/Sidebar";
import api from "../utils/axiosinstance";
import { UsersDto } from "../types/OrderTypes";

function UsersPage() {
  const [users, setUsers] = useState<UsersDto[]>([] || null);
  useEffect(() => {
    const fetchAccounts = async () => {
      const response = await api.get("Users/AllUsers");

      setUsers(response.data);
      console.log(response.data);
    };

    void fetchAccounts();

    return;
  }, []);
  return (
    <>
      <Sidebar />
      <div className="p-4 sm:ml-64">
        <div className="p-4 border-2 border-gray-200 border-dashed rounded-lg dark:border-gray-700 flex justify-center items-center Montserrat">
          <div>
            <div className="p-4">
              <div className="overflow-x-auto">
                <table className="table">
                  <thead className="bg-indigo-600 text-white font-bold text-left">
                    <tr>
                      <th className="px-4 py-2">First Name</th>
                      <th className="px-4 py-2">Last Name</th>
                      <th className="px-4 py-2">E-mail</th>
                    </tr>
                  </thead>
                  <tbody className="bg-white">
                    {users.map((user) => (
                      <tr className="border border-gray-200 font-bold">
                        <td className="px-4 py-2">{user.firstName}</td>
                        <td className="px-4 py-2">{user.lastName}</td>
                        <td className="px-4 py-2">{user.email}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

export default UsersPage;
