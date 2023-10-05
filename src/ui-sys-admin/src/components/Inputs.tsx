import Select from "react-select";
import { Controller, FieldValues } from "react-hook-form";
import { FormInputProps } from "types";
import { USStateSelectOptions } from "utils";

import style from 'styles/Inputs.module.css'

export function StandardInput<T extends FieldValues>(props: FormInputProps<T>) {
    const { label, name, register, component: Component } = props;
    return (
        <div className={style.inputWrapper}>
            <label htmlFor={name}>{label}</label>
            {Component ? <Component {...props} /> : <input {...register(name)} />}
        </div>
    )
}

export function StateSelect<T extends FieldValues>(props: FormInputProps<T>) {
    const { name, control } = props;
    return (
        <Controller
            name={name}
            control={control}
            render={({ field: { onChange, value, name, ref } }) => (
                <Select
                    name={name}
                    options={USStateSelectOptions}
                    onChange={onChange}
                />
            )}
        />
    )
}