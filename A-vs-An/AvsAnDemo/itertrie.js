function iterTrie(func) {
    function impl(prefix, node) {
        func(prefix, node);
        for (var c in node)
            if (c.length === 1)
                impl(prefix + c, node[c]);
    }
    impl("", AvsAn.raw);
}