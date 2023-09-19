import Select from "react-select";
import { Controller } from "react-hook-form";
import { InputProps } from "types";
import { stateSelectArray } from "utils";

import style from 'styles/Inputs.module.css'

export function StandardInput(props: InputProps) {
    const { label, name, type, required, register, component: Component } = props;
    return (
        <div className={style.inputWrapper}>
            <label htmlFor={name}>{label}</label>
            {Component ? <Component {...props} /> : <input {...register(name, { required })} />}
        </div>
    )
}

export function StateSelect({ control, name }: InputProps) {
    return (
        <Controller
            name={name}
            control={control}
            render={({ field }) => (
                <Select
                    {...field}
                    options={stateSelectArray}
                />
            )}
        />
    )
}