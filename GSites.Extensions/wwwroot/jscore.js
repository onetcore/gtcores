var body = document.getElementsByTagName('body')[0];
/**
 * 设置body属性。
 * @param {string} name 属性名称。
 * @param {*} value 属性值，如果为空则移除当前属性。
 * @returns
 */
export function setBodyAttribute(name, value) {
    if (typeof value === 'undefined')
        body.removeAttribute(name);
    else
        body.setAttribute(name, value);
}
