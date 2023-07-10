import { Fragment } from "react";
import { Disclosure, Menu, Transition } from "@headlessui/react";
import { Bars3Icon, BellIcon, XMarkIcon } from "@heroicons/react/24/outline";
import { NavLink } from "react-router-dom";

function classNames(...classes: string[]) {
  return classes.filter(Boolean).join(" ");
}
function Navbar() {
  return (
    <>
      <nav className="flex justify-around py-3 mx-auto bg-indigo-600">
        <div>
          <img className="h-8 w-auto" src="/apple-touch-icon.png" />
        </div>
       
        <div className="hidden sm:ml-6 sm:block">
          <div className="space-x-8 text-white navbar">
            <NavLink to="/">Home</NavLink>
            <NavLink to="/ordertrack">About</NavLink>
            <NavLink to="*">Contact US</NavLink>
          </div>
        </div>
      </nav>
    </>
  );
}

export default Navbar;
