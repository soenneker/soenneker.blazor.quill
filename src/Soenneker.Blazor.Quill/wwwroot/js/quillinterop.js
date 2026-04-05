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

export class QuillInterop {
    create(elementId, dotNetReference, options) {
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
    }

    destroy(elementId) {
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
    }

    getHtml(elementId) {
        const { quill } = getRequiredInstance(elementId);
        return quill.root?.innerHTML ?? "";
    }

    setHtml(elementId, html, source = "api") {
        const { quill } = getRequiredInstance(elementId);

        if (!html) {
            quill.setText("", source);
            return;
        }

        quill.clipboard.dangerouslyPasteHTML(html, source);
    }

    getText(elementId) {
        const { quill } = getRequiredInstance(elementId);
        return quill.getText();
    }

    setText(elementId, text, source = "api") {
        const { quill } = getRequiredInstance(elementId);
        quill.setText(text ?? "", source);
    }

    getContents(elementId) {
        const { quill } = getRequiredInstance(elementId);
        return JSON.stringify(quill.getContents());
    }

    setContents(elementId, contentsJson, source = "api") {
        const { quill } = getRequiredInstance(elementId);
        const contents = JSON.parse(contentsJson);
        quill.setContents(contents, source);
    }

    enable(elementId, enabled = true) {
        const { quill } = getRequiredInstance(elementId);
        quill.enable(enabled);
    }

    focus(elementId) {
        const { quill } = getRequiredInstance(elementId);
        quill.focus();
    }

    blur(elementId) {
        const { quill } = getRequiredInstance(elementId);
        quill.blur();
    }

    getSelection(elementId) {
        const { quill } = getRequiredInstance(elementId);
        return toSelectionRange(quill.getSelection());
    }

    setSelection(elementId, index, length = 0, source = "api") {
        const { quill } = getRequiredInstance(elementId);
        quill.setSelection(index, length, source);
    }
}

window.QuillInterop = new QuillInterop();
