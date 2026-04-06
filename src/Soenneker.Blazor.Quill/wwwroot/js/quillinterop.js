const quillInstances = new Map();

function getRequiredElement(elementId) {
    const element = document.getElementById(elementId);

    if (!element) {
        throw new Error(`Could not find Quill element '${elementId}'.`);
    }

    return element;
}

function getRequiredInstance(elementId) {
    const instance = quillInstances.get(elementId);

    if (!instance) {
        throw new Error(`Quill editor '${elementId}' has not been created.`);
    }

    return instance;
}

function normalizeOptions(options) {
    if (!options) {
        return {
            useCdn: true,
            theme: "snow",
            readOnly: false,
            manualCreate: false
        };
    }

    return {
        useCdn: options.useCdn ?? options.UseCdn ?? true,
        theme: options.theme ?? options.Theme ?? "snow",
        placeholder: options.placeholder ?? options.Placeholder ?? null,
        readOnly: options.readOnly ?? options.ReadOnly ?? false,
        bounds: options.bounds ?? options.Bounds ?? null,
        debug: options.debug ?? options.Debug ?? null,
        formats: options.formats ?? options.Formats ?? null,
        modules: options.modules ?? options.Modules ?? null,
        manualCreate: options.manualCreate ?? options.ManualCreate ?? false
    };
}

function toSelectionRange(range) {
    if (!range) {
        return null;
    }

    return {
        index: range.index ?? 0,
        length: range.length ?? 0
    };
}

function toTextChangePayload(quill, delta, oldDelta, source) {
    return {
        source,
        html: quill.root?.innerHTML ?? "",
        text: quill.getText(),
        contentsJson: JSON.stringify(quill.getContents()),
        deltaJson: JSON.stringify(delta),
        oldDeltaJson: JSON.stringify(oldDelta)
    };
}

function toSelectionChangePayload(range, oldRange, source) {
    return {
        range: toSelectionRange(range),
        oldRange: toSelectionRange(oldRange),
        source
    };
}

function disposeInstance(elementId) {
    const instance = quillInstances.get(elementId);

    if (!instance) {
        return;
    }

    instance.quill.off("text-change", instance.onTextChange);
    instance.quill.off("selection-change", instance.onSelectionChange);
    quillInstances.delete(elementId);
}

const interop = (() => {
    const instance = {};
    instance.create = function(elementId, dotNetReference, options) {
        disposeInstance(elementId);

        const normalized = normalizeOptions(options);
        const element = getRequiredElement(elementId);

        element.innerHTML = "";

        const quill = new Quill(element, {
            theme: normalized.theme,
            placeholder: normalized.placeholder ?? undefined,
            readOnly: normalized.readOnly,
            bounds: normalized.bounds ?? undefined,
            debug: normalized.debug ?? false,
            formats: normalized.formats ?? undefined,
            modules: normalized.modules ?? undefined
        });

        const onTextChange = (delta, oldDelta, source) => {
            dotNetReference.invokeMethodAsync("OnTextChanged", toTextChangePayload(quill, delta, oldDelta, source));
        };

        const onSelectionChange = (range, oldRange, source) => {
            dotNetReference.invokeMethodAsync("OnSelectionChanged", toSelectionChangePayload(range, oldRange, source));
        };

        quill.on("text-change", onTextChange);
        quill.on("selection-change", onSelectionChange);

        quillInstances.set(elementId, {
            quill,
            onTextChange,
            onSelectionChange,
            dotNetReference
        });

        dotNetReference.invokeMethodAsync("OnReady");
    };

    instance.destroy = function(elementId) {
        const instance = quillInstances.get(elementId);

        if (!instance) {
            return;
        }

        instance.quill.off("text-change", instance.onTextChange);
        instance.quill.off("selection-change", instance.onSelectionChange);
        instance.quill.root?.blur();

        const element = getRequiredElement(elementId);
        element.innerHTML = "";

        quillInstances.delete(elementId);
    };

    instance.getHtml = function(elementId) {
        const { quill } = getRequiredInstance(elementId);
        return quill.root?.innerHTML ?? "";
    };

    instance.setHtml = function(elementId, html, source = "api") {
        const { quill } = getRequiredInstance(elementId);

        if (!html) {
            quill.setText("", source);
            return;
        }

        quill.clipboard.dangerouslyPasteHTML(html, source);
    };

    instance.getText = function(elementId) {
        const { quill } = getRequiredInstance(elementId);
        return quill.getText();
    };

    instance.setText = function(elementId, text, source = "api") {
        const { quill } = getRequiredInstance(elementId);
        quill.setText(text ?? "", source);
    };

    instance.getContents = function(elementId) {
        const { quill } = getRequiredInstance(elementId);
        return JSON.stringify(quill.getContents());
    };

    instance.setContents = function(elementId, contentsJson, source = "api") {
        const { quill } = getRequiredInstance(elementId);
        const contents = JSON.parse(contentsJson);
        quill.setContents(contents, source);
    };

    instance.enable = function(elementId, enabled = true) {
        const { quill } = getRequiredInstance(elementId);
        quill.enable(enabled);
    };

    instance.focus = function(elementId) {
        const { quill } = getRequiredInstance(elementId);
        quill.focus();
    };

    instance.blur = function(elementId) {
        const { quill } = getRequiredInstance(elementId);
        quill.blur();
    };

    instance.getSelection = function(elementId) {
        const { quill } = getRequiredInstance(elementId);
        return toSelectionRange(quill.getSelection());
    };

    instance.setSelection = function(elementId, index, length = 0, source = "api") {
        const { quill } = getRequiredInstance(elementId);
        quill.setSelection(index, length, source);
    };

    return instance;
})();
export function create(elementId, dotNetReference, options) {
    return interop.create(elementId, dotNetReference, options);
}

export function destroy(elementId) {
    return interop.destroy(elementId);
}

export function getHtml(elementId) {
    return interop.getHtml(elementId);
}

export function setHtml(elementId, html, source = "api") {
    return interop.setHtml(elementId, html, source);
}

export function getText(elementId) {
    return interop.getText(elementId);
}

export function setText(elementId, text, source = "api") {
    return interop.setText(elementId, text, source);
}

export function getContents(elementId) {
    return interop.getContents(elementId);
}

export function setContents(elementId, contentsJson, source = "api") {
    return interop.setContents(elementId, contentsJson, source);
}

export function enable(elementId, enabled = true) {
    return interop.enable(elementId, enabled);
}

export function focus(elementId) {
    return interop.focus(elementId);
}

export function blur(elementId) {
    return interop.blur(elementId);
}

export function getSelection(elementId) {
    return interop.getSelection(elementId);
}

export function setSelection(elementId, index, length = 0, source = "api") {
    return interop.setSelection(elementId, index, length, source);
}
