/* eslint-disable @typescript-eslint/no-unsafe-argument */
/* eslint-disable @typescript-eslint/no-unsafe-member-access */
/* eslint-disable @typescript-eslint/no-unsafe-assignment */
import { useEffect, useState } from "react";
import Sidebar from "../components/Sidebar";
import api from "../utils/axiosinstance";

function Settings() {
  const [enableEmail, setEnableEmail] = useState<boolean | undefined>(
    undefined
  );
  const [enableToaster, setEnableToaster] = useState<boolean | undefined>(
    undefined
  );

  useEffect(() => {
    const fetchData = async () => {
      const response = await api.get("Users/Pull");
      const data = response.data;
      console.log(response.data);
      setEnableEmail(data.emailNotificationEnable);
      setEnableToaster(data.toasterNotificationEnable);
    };
    void fetchData();
  }, []);

  useEffect(() => {
    console.log(enableEmail);
    console.log(enableToaster);
  }, [enableEmail, enableToaster]);

  const handleEmailToggle = () => {
    setEnableEmail(!enableEmail);
  };

  const handleToasterToggle = () => {
    setEnableToaster(!enableToaster);
  };

  const handleSaveChanges = () => {
    const SetPreferences = async () => {
      const response = await api.put("Users", {
        emailNotificationsEnable: enableEmail,
        toasterNotificationsEnable: enableToaster,
      });
    };
    void SetPreferences();
    console.log(enableEmail, enableToaster);
  };

  return (
    <>
      <Sidebar />
      <div className="p-4 sm:ml-64 Montserrat">
        <div className="p-4 flex rounded-lg dark:border-gray-700">
          <div className="mx-auto w-1/2">
            <h2 className="p-5 text-left font-bold text-gray-500 border border-gray-500 rounded-t-lg">
              Notification Settings
            </h2>
            <div>
              <div className="p-5 border border-gray-500 border-t-0 dark:border-gray-700 dark:bg-gray-900">
                <p className="mb-2 text-gray-500 dark:text-gray-400">
                  If you want to e-mail notification with order details when an
                  order completed. Please enable e-mail notification.
                </p>
                <div className="mb-10 mt-5">
                  <input
                    id="EmailNotification"
                    type="checkbox"
                    className="checked:bg-blue-500 w-4 h-4 text-blue-600 bg-gray-100 border-gray-300 rounded focus:ring-blue-500 focus:ring-2"
                    checked={enableEmail}
                    onChange={handleEmailToggle}
                  />
                  <label
                    htmlFor="EmailNotification"
                    className="ml-2 text-sm font-medium text-gray-900 dark:text-gray-300"
                  >
                    Enable e-mail
                  </label>
                </div>

                <p className="text-gray-500 dark:text-gray-400">
                  If you want to be informed order completed and generated when
                  you wandering in application enable toast notification
                </p>
                <div className="mb-10 mt-5">
                  <input
                    id="EmailNotification"
                    type="checkbox"
                    className="checked:bg-blue-500 w-4 h-4 text-blue-600 bg-gray-100 border-gray-300 rounded focus:ring-blue-500 focus:ring-2"
                    checked={enableToaster}
                    onChange={handleToasterToggle}
                  />
                  <label
                    htmlFor="EmailNotification"
                    className="ml-2 text-sm font-medium text-gray-900 dark:text-gray-300"
                  >
                    Enable toast
                  </label>
                </div>

                <div className="forButtons">
                  <button
                    type="button"
                    className="inline-flex w-full justify-center rounded-md bg-blue-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-blue-500 sm:ml-3 sm:w-auto"
                    onClick={handleSaveChanges}
                  >
                    Save changes
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

export default Settings;
